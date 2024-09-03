import { useEffect, useState } from "react";
import ReceiptCard from "../components/ReceiptCard";
import { serverLink } from "../settings";
import translations from "../translations/pl.json";
import axios from 'axios';
import { Receipt } from "../models/Receipt";
import { Product } from "../models/Product";

function MainPage() {
  if (sessionStorage.getItem("userid") == null) {
    window.location.href = "/login";
  }

  const userID = sessionStorage.getItem("userid");
  const [receiptCards, setReceiptCards] = useState<JSX.Element[]>([]);
  const [thisMonthSpending, setThisMonthSpending] = useState<number>(0.0);

  async function fetchData(){
    const currentMonth = new Date().getMonth()+1;
    const startDate = parseToDate(new Date(new Date().getFullYear(), currentMonth, 1));
    const endDate = parseToDate(new Date(new Date().getFullYear(), currentMonth + 1, 1));
    try{
      const response = await axios.get(`${serverLink}/Receipt/mainPageData/${startDate}/${endDate}/${userID}`);
      if(response.status !== 200){
        throw new Error('Network response was not ok');
      }
      const lastFourReceipts = response.data.receipts.slice(0, Math.min(response.data.receipts.length, 4));
      const lastMonthSpendings = response.data.total;
      return {lastFourReceipts, lastMonthSpendings};
    }
    catch(error){
      alert('There was a problem getting your data');
      return null;
    }
  }

  async function getReceiptProducts(receiptID: number): Promise<Product[] | null> {
    try {
      const response = await axios.get(`${serverLink}/Product/receipt/${receiptID}`);
      if (response.status !== 200) {
        throw new Error('Network response was not ok');
      }
      if (response.data.length === 0) {
        return [];
      }
      return await response.data;
    } catch (error) {
      alert('There was a problem getting products for your receipts');
      return null;
    }
  }

  async function getReceiptItemsAndTotal(receiptID: number) {
    const products = await getReceiptProducts(receiptID);
    if (products === null) {
      return { items: [], total: 0.0 };
    }
    let total = 0.0;
    products.forEach((product) => {
      total += product.price
    });

    return { products, total };
  }

  function parseToDate(date: Date){
    const _date = new Date(date);
    const day = _date.getDate();
    const month = _date.getMonth();
    const year = _date.getFullYear();
    return `${year}-${month}-${day}`;
  }

  useEffect(() => {
    const fetchDataForMainPage = setTimeout(async () => {
      const data = await fetchData();
      if(data === null){
        return;
      }
      const receipts = data.lastFourReceipts;
      const lastMonthSpendings = data.lastMonthSpendings;
      console.log(lastMonthSpendings);
      const generatedCards = await Promise.all(receipts.map(async (receipt: Receipt) => {
        const { items, total } = await getReceiptItemsAndTotal(receipt.id);
        return (
          <ReceiptCard
            key={receipt.id}
            ShopName={receipt.shopName}
            ReceiptDate={receipt.date}
            Total={total}
            Items={items}
          />
        );
      }));
      setReceiptCards(generatedCards);
      setThisMonthSpending(lastMonthSpendings);
    }, 150);
    return () => clearTimeout(fetchDataForMainPage);
  }, []);

  return (
    <>
      <p>{translations.youSpentThisMonth} {thisMonthSpending}</p>
      <p>{translations.lastFourReceipts}</p>
      <div className="flex wrap overflow-hidden flex-wrap">
        {receiptCards}
      </div>
    </>
  )
}

export default MainPage;
