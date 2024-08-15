import ReceiptCard from "../components/ReceiptCard";
import { serverLink } from "../settings";
import translations from "../translations/pl.json";

function MainPage() {
  if(sessionStorage.getItem("userid") == null){
    window.location.href = "/login";
  }
  const userID = sessionStorage.getItem("userid");

  async function getLastFourReceipts(){
    try {
        const response = await fetch(`${serverLink}/Receipt/receipts/${userID}`, {
            method: 'GET', 
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const data = await response.json();

        if (response.ok) {
            // return first 4 elements of data
            return data.slice(0, 4);
        } else {
            // show error message
            alert('Invalid username or password');
        }
        return null;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        alert('Failed to login. Please try again later.');
        return null;
    }
  }

  async function generateReceiptCards(){
    let cards = [];
    let receipts = await getLastFourReceipts();
    if(receipts === null){
      return;
    }
    for(let i = 0; i < receipts.length; i++){
      let receipt = receipts[i];
      cards.push(<ReceiptCard ShopName={receipt.shopName} Date={receipt.date} Total={receipt.total} Items={receipt.items} />);
    }
    return cards;
  }

  function parseToDate(date: Date){
    let day = date.getDate();
    let month = date.getMonth();
    let year = date.getFullYear();
    return `${day}-${month}-${year}`;
  }

  async function thisMonthSpending(){
    const currentMonth = new Date().getMonth();
    const startDate = parseToDate(new Date(new Date().getFullYear(), currentMonth, 1));
    const endDate = parseToDate(new Date(new Date().getFullYear(), currentMonth + 1, 1));

    try {
        const response = await fetch(`${serverLink}/Receipt/totalSpending/${startDate}/${endDate}/${userID}`, {
            method: 'GET', 
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const data = await response.json();

        if (response.ok) {
            return data;
        } else {
            alert('Invalid username or password');
        }
        return null;
    } catch (error) {
        console.error('There was a problem with the fetch operation:', error);
        alert('Failed to login. Please try again later.');
        return null;
    }
  }

  let cards;
  generateReceiptCards().then((c) => {
    cards = c;
  }) //generate from this
  let thisMonthSpendingResult = 0.0;
  thisMonthSpending().then((result) => {
    thisMonthSpendingResult = result;
  });

  return (
    <>
    <p>{translations.lastFourReceipts}</p>
      <div className="flex wrap overflow-hidden flex-wrap">
        {cards}
      </div>
      <p>{translations.youSpentThisMonth} {thisMonthSpendingResult}</p>
    </>
  )
}

export default MainPage;
