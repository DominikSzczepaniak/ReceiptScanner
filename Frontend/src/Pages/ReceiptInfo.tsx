import ReceiptInfoTable from "@/components/ReceiptInfoTable.tsx";
import {useParams} from "react-router-dom";

function ReceiptInfo(){
    //show info of every product on receipt 
    //give total sum
    //if categories are set show a graph that presents how much was spent in % or value (pie chart graph)
    //this is basically another table with some graphs at the bottom

    //add button to delete info, create something like a navbar with options

    //add return to receiptTable button if came from there
    const { id } = useParams<{id: string}>();
    const receiptInfoId = id as unknown as number;
    return (<ReceiptInfoTable id={receiptInfoId} />);
}

export default ReceiptInfo;