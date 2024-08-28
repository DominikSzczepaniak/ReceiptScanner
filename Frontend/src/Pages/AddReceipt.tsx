import { useState } from "react";
import { serverLink } from "../settings";

export default function AddReceipt(){
    const [state, setState] = useState<{file: FormData | null}>({file: null});


    const handleFileImport = (e: React.ChangeEvent<HTMLInputElement>) => {
        e.preventDefault();
        if(e.target.files === null){
            return;
        }
        let file = e.target.files[0];
        let form = new FormData();
        form.append('file', file);
        setState({file: form});
    }

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        await fetch(`${serverLink}/upload`, {
            method: 'POST',
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