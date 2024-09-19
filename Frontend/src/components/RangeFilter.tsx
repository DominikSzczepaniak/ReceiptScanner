import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card.tsx";
import translations from "../translations/pl.json";
import { Label } from "@/components/ui/label.tsx";
import { Input } from "@/components/ui/input.tsx";
import { useState } from "react";
import { v4 as uuid } from 'uuid';
import { Button } from "@/components/ui/button.tsx";

type NumericKeys<T> = {
    [K in keyof T]: T[K] extends number ? K : never;
}[keyof T];

interface RangeFilterProps<T> {
    min: number,
    max: number,
    step?: number,
    addFilter: (key: string, fn: (items: T) => boolean) => void,
    deleteFilter: (key: string) => void,
    filterField: NumericKeys<T>,
    applyFilters: () => void;
}

const RangeFilter = <T, >({
                              min,
                              max,
                              step = 1,
                              addFilter,
                              deleteFilter,
                              filterField,
                              applyFilters
                          }: RangeFilterProps<T>) => {
    const [minValue, setMinValue] = useState(min);
    const [maxValue, setMaxValue] = useState(max);
    const [filterId] = useState(uuid());

    const handleSave = () => {
        processFilter(minValue, maxValue);
    };

    const processFilter = (newMinValue: number, newMaxValue: number) => {
        deleteFilter(filterId);
        if (newMinValue > newMaxValue) {
            return;
        }
        addFilter(filterId, (item: T) => {
            const fieldValue = item[filterField] as number;
            return fieldValue >= newMinValue && fieldValue <= newMaxValue;
        });
        applyFilters();
    };
    //TODO add popover from shadcn
    return (
        <Card className="w-full max-w-sm">
            <CardHeader>
                <CardTitle>{translations.filters.rangeFilterTitle}</CardTitle>
            </CardHeader>
            <CardContent>
                <div className="grid grid-cols-2 gap-4">
                    <div className="space-y-2">
                        <Label htmlFor="min-value">{translations.filters.minimum}</Label>
                        <Input
                            id="min-value"
                            type="number"
                            min={min}
                            max={maxValue}
                            step={step}
                            value={minValue}
                            onChange={(e) => setMinValue(Number(e.target.value))}
                        />
                    </div>
                    <div className="space-y-2">
                        <Label htmlFor="max-value">{translations.filters.maximum}</Label>
                        <Input
                            id="max-value"
                            type="number"
                            min={minValue}
                            max={max}
                            step={step}
                            value={maxValue}
                            onChange={(e) => setMaxValue(Number(e.target.value))}
                        />
                    </div>
                </div>
                <Button onClick={handleSave}>{translations.common.save}</Button>
            </CardContent>
        </Card>
    );
};

export default RangeFilter;
