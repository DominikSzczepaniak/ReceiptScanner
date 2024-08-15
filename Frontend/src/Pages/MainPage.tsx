import { useEffect, useState } from "react";
import ReceiptCard from "../components/ReceiptCard";
import { serverLink } from "../settings";
import translations from "../translations/pl.json";
import axios from 'axios';
import { Receipt } from "../models/Receipt";
import { Product } from "../models/Product";
import {debounce} from "../debouncing";

function MainPage() {
  if (sessionStorage.getItem("userid") == null) {
    window.location.href = "/login";
  }

  let debounceTimeout = 0;

  const fetchData = () => {
    if (debounceTimeout) {
      clearTimeout(debounceTimeout);
    }

    debounceTimeout = setTimeout(() => {
      // Wywołanie API
    }, 300); // Opóźnienie 300 ms
  };

  const userID = sessionStorage.getItem("userid");
  const [receiptCards, setReceiptCards] = useState<JSX.Element[]>([]);

  async function getLastFourReceipts() {
    try {
      const response = await axios.get(`${serverLink}/Receipt/receipts/${userID}`);
      if (response.status !== 200) {
        throw new Error('Network response was not ok');
      }
      const data = await response.data;

      return data.slice(0, Math.min(data.length, 4));
    } catch (error) {
      console.error('There was a problem with the fetch operation:', error);
      alert('Failed to login. Please try again later.');
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
      const data = await response.data;

      return data;
    } catch (error) {
      console.error('There was a problem with the fetch operation:', error);
      alert('Failed to login. Please try again later.');
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

  // function parseToDate(date: Date){
  //   let _date = new Date(date);
  //   let day = _date.getDate();
  //   let month = _date.getMonth();
  //   let year = _date.getFullYear();
  //   return `${year}-${month}-${day}`;
  // }

  // async function thisMonthSpending(){
  //   const currentMonth = new Date().getMonth();
  //   const startDate = parseToDate(new Date(new Date().getFullYear(), currentMonth, 1));
  //   const endDate = parseToDate(new Date(new Date().getFullYear(), currentMonth + 1, 1));

  //   try {
  //       const response = await fetch(`${serverLink}/Receipt/totalSpending/${startDate}/${endDate}/${userID}`, {
  //           method: 'GET', 
  //           headers: {
  //               'Content-Type': 'application/json',
  //           },
  //       });

  //       if (!response.ok) {
  //           throw new Error('Network response was not ok');
  //       }

  //       const data = await response.json();

  //       if (response.ok) {
  //           return data;
  //       } else {
  //           alert('Invalid username or password');
  //       }
  //       return null;
  //   } catch (error) {
  //       console.error('There was a problem with the fetch operation:', error);
  //       alert('Failed to login. Please try again later.');
  //       return null;
  //   }
  // }


  useEffect(() => {
    async function fetchReceipts() {
      const receipts = await getLastFourReceipts(); //TODO debouncing
      if (receipts === null) {
        return;
      }
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
    }
    fetchReceipts();
  }, []);


  // let thisMonthSpendingResult = 0.0;
  // thisMonthSpending().then((result) => {
  //   thisMonthSpendingResult = result;
  // });

  let thisMonthSpendingResult = 0.0;

  return (
    <>
      <p>{translations.lastFourReceipts}</p>
      <div className="flex wrap overflow-hidden flex-wrap">
        {receiptCards}
      </div>
      <p>{translations.youSpentThisMonth} {thisMonthSpendingResult}</p>
    </>
  )
}

export default MainPage;
