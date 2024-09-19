import {useEffect, useState} from "react";
import {serverLink} from "../settings";
import {Receipt} from "../models/Receipt";
import {Table, TableBody, TableCaption, TableCell, TableHead, TableHeader, TableRow,} from "@/components/ui/table";
import translations from "../translations/pl.json";
import {getReceiptTotal} from "@/data/ReceiptData.ts";
import {parseDate} from "@/helpers/parseDate.ts";

function ReceiptTable() {
    const [receipts, setReceipts] = useState<Receipt[]>([]);
    const [receiptTotals, setReceiptTotals] = useState<{ [key: number]: number }>({});
    const [isLoading, setIsLoading] = useState(true);

    const userId = sessionStorage.getItem('userid')! as unknown as number;

    const getReceiptsForUser = async (id: number) => {
        let result: Receipt[] = [];
        await fetch(`${serverLink}/Receipt/receipts/${id}`)
            .then((response) => response.json())
            .then((data) => (result = data))
            .catch((error) => console.log(error))
        return result;
    };

    const fetchReceiptTotals = async (receipts: Receipt[]): Promise<void> => {
        const totals: { [key: number]: number } = {};
        for (const receipt of receipts) {
            totals[receipt.id] = await getReceiptTotal(receipt.id);
        }
        setReceiptTotals(totals);
    };

    useEffect(() => {
        if(userId && isLoading) {
            getReceiptsForUser(userId).then((data) => {
                setReceipts(data);
            })
                .catch(error => console.error(error))
                .finally(() => setIsLoading(false));
        }
    }, [userId, isLoading]);

    useEffect(() => {
        if (receipts.length > 0) {
            fetchReceiptTotals(receipts);
        }
    }, [receipts]);

    return (
        <div>
            <Table>
                <TableCaption>{translations.listOfReceipts}</TableCaption>
                <TableHeader>
                    <TableRow>
                        <TableHead className="w-[100px]">{translations.receipts.date}</TableHead>
                        <TableHead>{translations.receipts.shopName}</TableHead>
                        <TableHead>{translations.receipts.priceAmount}</TableHead>
                        <TableHead className="text-right">{translations.receipts.receiptInfo}</TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {receipts.map((receipt) => (
                        <TableRow key={receipt.id}>
                            <TableCell>{parseDate(receipt.date)}</TableCell>
                            <TableCell>{receipt.shopName[1].toUpperCase() + receipt.shopName.toString().slice(2, receipt.shopName.length-1)}</TableCell>
                            <TableCell>{receiptTotals[receipt.id] || 'Loading...'}</TableCell>
                            <TableCell className="text-right font-bold">
                                <a href={`/Receipt/${receipt.id}`}>{translations.receipts.receiptInfo}</a>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </div>
    );
}

export default ReceiptTable;
