import { useEffect, useState } from "react";
import ReceiptCard from "../components/ReceiptCard";
import { serverLink } from "../settings";
import translations from "../translations/pl.json";
import { Receipt } from "../models/Receipt";
import {getReceiptItemsAndTotal} from "@/data/ReceiptData.ts";
import {parseDate} from "@/helpers/parseDate.ts";

function MainPage() {
  if (sessionStorage.getItem("userid") == null) {
    window.location.href = "/login";
  }
  const userID = sessionStorage.getItem("userid");
  const [receiptCards, setReceiptCards] = useState<JSX.Element[]>([]);
  const [thisMonthSpending, setThisMonthSpending] = useState<number>(0.0);

  async function fetchData(){
    const currentMonth = new Date().getMonth()+1;
    const startDate = parseDate(new Date(new Date().getFullYear(), currentMonth, 1));
    const endDate = parseDate(new Date(new Date().getFullYear(), currentMonth + 1, 1));
    try{
      console.log(`${serverLink}/Receipt/mainPageData/${startDate}/${endDate}/${userID}`);
      const response = await fetch(`${serverLink}/Receipt/mainPageData/${startDate}/${endDate}/${userID}`);
      if(!response.ok){
        throw new Error('Network response was not ok');
      }
      const data = await response.json();
      const lastFourReceipts = data.receipts.slice(0, Math.min(data.receipts.length, 4));
      const lastMonthSpendings = data.total;
      return {lastFourReceipts, lastMonthSpendings};
    }
    catch(error){
      console.error('There was a problem getting your data: ', error);
      return null;
    }
  }

  useEffect(() => {
    const fetchDataForMainPage = setTimeout(async () => {
      const data = await fetchData();

      if(data === null){
        return;
      }

      const receipts = data.lastFourReceipts;
      const lastMonthSpendings = data.lastMonthSpendings;
      try{
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
      } catch(error) {
        console.error('There was a problem getting your data: ', error);
        return null;
      }
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
