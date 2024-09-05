import {Product} from "@/models/Product.ts";
import {serverLink} from "@/settings.ts";

export async function getReceiptProducts(receiptID: number): Promise<Product[] | null> {
    const response = await fetch(`${serverLink}/Product/receipt/${receiptID}`);

    if (!response.ok) {
        throw new Error('Network response was not ok');
    }

    const data = await response.json();

    if (data.length === 0) {
        return [];
    }

    return data;
}

export async function getReceiptItemsAndTotal(receiptID: number) {
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

export async function getReceiptTotal(receiptID: number) {
    const data = await getReceiptItemsAndTotal(receiptID);
    return data.total;
}