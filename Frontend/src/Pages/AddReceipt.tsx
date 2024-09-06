import {useRef, useState} from "react";
import { serverLink } from "../settings";
import { v4 as uuid } from 'uuid';
import translations from "../translations/pl.json";
import { Button } from "@/components/ui/button"
import {
    Card,
    CardContent,
    CardDescription,
    CardFooter,
    CardHeader,
    CardTitle,
} from "@/components/ui/card"
import { Label } from "@/components/ui/label"
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select"
import { useToast } from "@/hooks/use-toast"

export default function AddReceipt() {
    const {toast} = useToast();
    const [state, setState] = useState<{ file: FormData | null }>({file: null});
    const [selectPlaceholder, setSelectPlaceholder] = useState("🇵🇱, Polski"); //TODO change to user website language (onLoad run setSelectPlaceholder on correct value)
    const [selectedLanguage, setSelectedLanguage] = useState<string>("🇵🇱, Polski");
    const fileInputRef = useRef<HTMLInputElement>(null);

    const supportedLanguages = ["🇵🇱, Polski", "🇬🇧, English"];

    const divElement = state.file === null ?
        <span className="text-black">{translations.addReceiptPage.clickToSendPhoto}</span> :
        <span className="text-black">{translations.addReceiptPage.successfullySentPhoto}</span>; //TODO - show image there


    const handleFileImport = (e: React.ChangeEvent<HTMLInputElement>) => {
        e.preventDefault();
        if (e.target.files === null) {
            return;
        }
        const file = e.target.files[0];
        const form = new FormData();
        form.append('file', file);
        setState({file: form});
    }

    const handleSubmit = async () => {
        state.file!.append('language', selectedLanguage);

        const imageId = uuid();
        await fetch(`${serverLink}/uploadImage/${imageId}`, {
            method: 'POST',
            mode: 'cors',
            body: state.file
        })
        .then(response => response.json())
        .then(data => {
            console.log(data);
        })
    }

    const handleButtonClick = (e: React.MouseEvent<HTMLButtonElement>) => {
        e.preventDefault();
        if (!state.file) {
            return (toast({
                title: translations.addReceiptPage.failure,
                description: translations.addReceiptPage.fileNotSelected,
                className: "border-2 border-red-400"
            }))
        }
        handleSubmit().then(() => {
            return (toast({
                title: translations.addReceiptPage.success,
                description: translations.addReceiptPage.successfullyAddedReceipt
            }))
        }).catch((error) => {
            console.error(error);
            return (toast({
                title: translations.addReceiptPage.failure,
                description: translations.addReceiptPage.couldntAddReceipt,
                className: "border-2 border-red-400"
            }))
        });
    }

    const triggerFileInput = () => {
        if (fileInputRef.current) {
            fileInputRef.current.click();
        }
    };

    return (
        <div className="flex content-center justify-center items-center h-screen">
            <Card className="w-3/4 mt-2.5">
                <CardHeader>
                    <CardTitle>{translations.addReceiptPage.addReceipt}</CardTitle>
                    <CardDescription>{translations.addReceiptPage.addReceiptDescription}</CardDescription>
                </CardHeader>
                <CardContent>
                    <form>
                        <div className="grid w-full items-center gap-4">
                            <div className="flex flex-col space-y-1.5">
                                <Label htmlFor="language">{translations.addReceiptPage.chooseLanguage}</Label>
                                <Select onValueChange={setSelectedLanguage}>
                                    <SelectTrigger id="language">
                                        <SelectValue placeholder={selectPlaceholder}/>
                                    </SelectTrigger>
                                    <SelectContent position="popper">
                                        {supportedLanguages.map((language, index) =>
                                            <SelectItem key={`${language}-${index}`}
                                                        value={language}>{language}</SelectItem>
                                        )}
                                    </SelectContent>
                                </Select>
                            </div>
                        </div>
                        <div
                            onClick={triggerFileInput}
                            className="w-full h-[350px] border rounded border-primary mt-2.5 bg-slate-200 flex justify-center items-center cursor-pointer"
                        >
                            {divElement}
                        </div>

                        <input
                            ref={fileInputRef}
                            type="file"
                            accept="image/*"
                            onChange={handleFileImport}
                            style={{display: 'none'}}
                        />
                    </form>
                </CardContent>
                <CardFooter className="flex justify-between">
                    <Button className="w-full" onClick={handleButtonClick}>Send</Button>
                </CardFooter>
            </Card>
        </div>
    );
}