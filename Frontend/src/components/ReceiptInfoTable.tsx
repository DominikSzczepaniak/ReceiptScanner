import {useEffect, useState} from "react";
import {Product} from "@/models/Product.ts";
import {serverLink} from "@/settings.ts";
import {Table, TableBody, TableCell, TableHead, TableHeader, TableRow} from "@/components/ui/table.tsx";
import translations from "../translations/pl.json";
import RangeFilter from "@/components/RangeFilter.tsx";
import {Button} from "@/components/ui/button.tsx";
import {Tabs, TabsContent, TabsList, TabsTrigger} from "@/components/ui/tabs.tsx";

type ReceiptInfoTableProps = {
    id: number;
}

interface Filter{
    key: string;
    fn: (products: Product) => boolean;
}

function ReceiptInfoTable(props: ReceiptInfoTableProps) {
    const [allProducts, setAllProducts] = useState<Product[]>([]);
    const [products, setProducts] = useState<Product[]>([]);
    const [filters, setFilters] = useState<Filter[]>([]);

    const addFilter = (key: string, fn: (products: Product) => boolean) => {
        setFilters((filters) => {
            return [...filters, {key, fn}];
        });
    };

    const deleteFilter = (key: string) => {
        setFilters((prevFilters) => {
            return prevFilters.filter(f => f.key !== key);
        });
    }

    const fetchProducts = async (): Promise<Product[]> => {
        return await fetch(`${serverLink}/Product/receipt/${props.id}`)
            .then((response) => response.json())
            .then((data) => {
                return data;
            });
    }

    const showTabsMenuForFilters = () => {
        const tabsSelector = document.getElementById("tabs")!;
        tabsSelector.className === "hidden" ? tabsSelector.className = "" : tabsSelector.className = "hidden";
    }

    const applyFilters = () => {
        let filteredProducts = [...allProducts];
        filters.forEach(filter => {
            filteredProducts = filteredProducts.map(item => {
                const result = filter.fn(item);
                return result ? item : null;
            }).filter((item): item is Product => item !== null);
        });
        setProducts(filteredProducts);
    }

    useEffect(() => {
        fetchProducts().then((data) => setAllProducts(data));
    }, []);

    useEffect(() => {
        applyFilters();
    }, [allProducts, filters]);

    return (
        <div>
            <Table>
                <TableHeader>
                    <TableRow>
                        <TableHead>{translations.products.productName}</TableHead>
                        <TableHead>{translations.products.amount}</TableHead>
                        <TableHead>{translations.products.category}</TableHead>
                        <TableHead className="text-right">{translations.products.price}</TableHead>
                        <div>
                            <Button onClick={showTabsMenuForFilters}></Button>
                            <Tabs className="hidden" id="tabs">
                                <TabsList>
                                    <TabsTrigger value="priceRange">{translations.filters.prices}</TabsTrigger>
                                    <TabsTrigger value="quantityRange">{translations.filters.quantities}</TabsTrigger>
                                    <TabsTrigger value="categoryPicker">{translations.filters.categories}</TabsTrigger>
                                </TabsList>
                                <TabsContent value="priceRange">
                                    <RangeFilter<Product> min={0} max={1000}
                                                          step={10}
                                                          addFilter={addFilter}
                                                          deleteFilter={deleteFilter}
                                                          filterField="price"
                                                            applyFilters={applyFilters}/>
                                </TabsContent>
                            </Tabs>
                        </div>
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