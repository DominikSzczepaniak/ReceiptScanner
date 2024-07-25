interface ReceiptItem{
    Name: string;
    Price: number;
    Amount: string;
}

interface ReceiptCardProps{
    ShopName: string;
    Date: string;
    Total: number;
    Items: Array<ReceiptItem>;
}


function ReceiptCard(props: ReceiptCardProps){
    const {ShopName, Date, Total, Items} = props;
    let screenWidth = window.innerWidth;
    let sideBarWidth = screenWidth * 0.2;
    let mainSize = screenWidth - sideBarWidth - 20;
    console.log(mainSize)
    let marginSize = 32;
    let cardWidth = (mainSize - marginSize*4)/4;
    console.log(cardWidth);
    let numberOfCards = [1,2,3,4,5,6]
    //margin takes 

    return(
        <div className={`h-1/4 p-4 rounded-lg border-8 border-solid border-primary-color bg-secondary-color text-text-color m-4`} style={{width: cardWidth+"px"}}>
            <div className="receipt-card-header">
                <h3>{ShopName}</h3>
                <p>{Date}</p>
                <h4>Total: {Total}</h4>
            </div>
            <div className="receipt-card-body">
                <ul>
                    {Items.map((item, index) => (
                        <li key={index}>{item['Name']} - {item['Price']} - {item['Amount']}</li>
                    ))}
                </ul>
            </div>
        </div>
    )
    
}

export default ReceiptCard;