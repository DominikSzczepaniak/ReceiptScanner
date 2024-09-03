import { useState } from "react";
import { serverLink } from "../settings";
import { v4 as uuid } from 'uuid';

export default function AddReceipt(){
    const [state, setState] = useState<{file: FormData | null}>({file: null});


    const handleFileImport = (e: React.ChangeEvent<HTMLInputElement>) => {
        e.preventDefault();
        if(e.target.files === null){
            return;
        }
        const file = e.target.files[0];
        const form = new FormData();
        form.append('file', file);
        setState({file: form});
    }

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
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
        .catch(error => {
            console.error('Error:', error);
        });
    }

    return (
        <>
            <div>
                <form onSubmit={handleSubmit} className="w-1/3 h-1/3">
                    <input type="file" accept="image/*" onChange={handleFileImport}/>
                    <button type="submit">Submit</button>
                </form>
            </div>
        </>
    );
}