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

  async function getLastMonthSpending(){
    const currentMonth = new Date().getMonth()+1;
    const startDate = parseDate(new Date(new Date().getFullYear(), currentMonth, 1));
    const endDate = parseDate(new Date(new Date().getFullYear(), currentMonth + 1, 1));
    try{
      const response = await fetch(`${serverLink}/Receipt/mainPageData/${startDate}/${endDate}/${userID}`);
      if(!response.ok){
        throw new Error('Network response was not ok');
      }
      const data = await response.json();
      const lastMonthSpendings = data.total;
      return lastMonthSpendings;
    }
    catch(error){
      console.error('There was a problem getting your data: ', error);
      return null;
    }
  }

  async function getLastFourReceipts(){
    try{
      const response = await fetch(`${serverLink}/Receipt/receipts/${userID}`);
      if(!response.ok){
        throw new Error('Network response was not ok');
      }
      const data = await response.json();
      const lastFourReceipts = data.slice(0, Math.min(data.length, 4));
      return lastFourReceipts;
    }
    catch(error){
      console.error('There was a problem getting your data: ', error);
      return null;
    }
  }

  async function fetchData(){
    const lastFourReceipts = await getLastFourReceipts();
    const lastMonthSpendings = await getLastMonthSpending();
    return {lastFourReceipts, lastMonthSpendings};
  }

  async function generateReceiptCards(data: {lastFourReceipts: Receipt[], lastMonthSpendings: number}){
    const receipts = data.lastFourReceipts;
    const lastMonthSpendings = data.lastMonthSpendings;
    try {
      const generatedCards = await Promise.all(receipts.map(async (receipt: Receipt) => {
        const {items, total} = await getReceiptItemsAndTotal(receipt.id);
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
    } catch (error) {
      console.error('There was a problem getting your data: ', error);
      return null;
    }
  }

  useEffect(() => {
    fetchData().then(data => {
      if(data === null){
        return;
      }
      generateReceiptCards(data);
    });
  }, []);

  return (
    <div>
      <p>{translations.youSpentThisMonth} {thisMonthSpending}</p>
      <p>{translations.lastFourReceipts}</p>
      <div className="flex wrap overflow-hidden flex-wrap">
        {receiptCards}
      </div>
    </div>
  )
}

export default MainPage;
