import React, { useEffect, useState } from "react";

interface ReceiptItem {
    Name: string;
    Price: number;
    Amount: string;
}

interface ReceiptCardProps {
    ShopName: string;
    ReceiptDate: Date;
    Total: number;
    Items?: Array<ReceiptItem>;
}

const ReceiptCard: React.FC<ReceiptCardProps> = ({ ShopName, ReceiptDate, Total, Items }) => {
    const [cardWidth, setCardWidth] = useState(0);

    const calcNumberOfCards = (width: number) => {
        if (width > 1024) {
            return 4;
        } else if (width > 768) {
            return 3;
        } else if (width > 640) {
            return 2;
        } else {
            return 1;
        }
    }

    const calculateCardWidth = () => {
        const screenWidth = window.innerWidth;
        const sideBarWidth = screenWidth * 0.2;
        const mainSize = screenWidth - sideBarWidth - 20;
        const marginSize = 32;
        const numberOfCards = calcNumberOfCards(screenWidth);
        return (mainSize - marginSize * numberOfCards) / numberOfCards - 5;
    }

    useEffect(() => {
        const handleResize = () => {
            setCardWidth(calculateCardWidth());
        };

        setCardWidth(calculateCardWidth());

        window.addEventListener('resize', handleResize);
        
        return () => {
            window.removeEventListener('resize', handleResize);
        };
    }, []);

    const _date = new Date(ReceiptDate);
    const day = _date.getDay();
    const month = _date.getMonth();
    const year = _date.getFullYear();
    const dateString = `${year}-${month}-${day}`;


    return (
        <div className={`h-1/4 p-4 rounded-lg border-8 border-solid border-primary-color bg-secondary-color text-text-color m-4`} style={{ width: cardWidth + "px" }}>
            <div className="receipt-card-header">
                <h3>{ShopName}</h3>
                <p>{dateString}</p>
                <h4>Total: {Total}</h4>
            </div>
            <div className="receipt-card-body">
                <ul>
                    {Items?.map((item, index) => (
                        <li key={index}>{item['Name']} - {item['Price']} - {item['Amount']}</li>
                    ))}
                </ul>
            </div>
        </div>
    )
}

export default ReceiptCard;
