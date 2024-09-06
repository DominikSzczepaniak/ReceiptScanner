import {useEffect, useState} from "react";
import {Product} from "@/models/Product.ts";
import {serverLink} from "@/settings.ts";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import translations from "../translations/pl.json";

type ReceiptInfoTableProps = {
    id: number;
}

function ReceiptInfoTable(props: ReceiptInfoTableProps) {
    const [products, setProducts] = useState<Product[]>([]);

    const fetchProducts = async (): Promise<Product[]> => {
        return await fetch(`${serverLink}/Product/receipt/${props.id}`)
            .then((response) => response.json())
            .then((data) => {
                return data;
            });
    }

    useEffect(() => {
        fetchProducts().then((data) => setProducts(data));
    }, []);

    return (
        <div>
            <Table>
                <TableHeader>
                    <TableRow>
                        <TableHead>{translations.products.productName}</TableHead>
                        <TableHead>{translations.products.amount}</TableHead>
                        <TableHead>{translations.products.category}</TableHead>
                        <TableHead className="text-right">{translations.products.price}</TableHead>
                    </TableRow>
                </TableHeader>
                <TableBody>
                    {products.map((product) => (
                        <TableRow key={product.id}>
                            <TableCell>{product.name[0].toUpperCase() + product.name.slice(1, product.name.length)} </TableCell>
                            <TableCell>{product.quantityWeight}</TableCell>
                            <TableCell>{product.category ? product.category : translations.products.noCategory}</TableCell>
                            <TableCell className="text-right">{product.price}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </div>
    )
}

export default ReceiptInfoTable;