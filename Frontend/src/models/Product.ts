export interface Product{
    id?: number;
    name: string;
    quantityWeight: number;
    price: number;
    ownerId: number;
    category?: string;
}