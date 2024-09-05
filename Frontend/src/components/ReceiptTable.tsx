import { useEffect, useState } from "react";
import { serverLink } from "../settings";
import { Receipt } from "../models/Receipt";

function ReceiptTable() {
    const [receipts, setReceipts] = useState<Receipt[]>([]);

    const userId = sessionStorage.getItem('userid')! as unknown as number;

    const getReceiptsForUser = async (id: number) => {
        let result: Receipt[] = [];
        await fetch(`${serverLink}/Receipt/receipts/${id}`)
        .then(response => response.json())
        .then(data => result = data)
        .catch(error => console.log(error));
        return result;
    }

    useEffect(() => {
        getReceiptsForUser(userId).then(data => setReceipts(data));
    }, [receipts]);


    //return table for those receipts
    return (
    <div>
        
    </div>
    )
}

export default ReceiptTable;