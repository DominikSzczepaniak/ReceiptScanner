import ReceiptTable from "@/components/ReceiptTable.tsx";

function MyReceipts() {
    //return a table with columns: [date, total sum, shopname, link to receipt info]
    //allow sum, date and shopname to be sorted with default being sorting by date 
    //allow filtering by shopname, date (between) and sum

    //allow multiple choice and actions for receipts like deletion with confirmation (always double ask, on default ask third time "this operation is permanent" that is possible to delete)
    //user story:
    //I'm clicking checkboxes next to receipts i want to delete. I click delete button. I'm asked again in a popup if i want to delete those receipts. I click yes. Then im asked again "This operation is permanent. Proceed?" with options: Yes, Yes and never ask again, No. If i click on Yes and never ask again i don't ever want to be asked this question again and it's linked to account im logged into.
    return (
        <ReceiptTable />
    );
}

export default MyReceipts;