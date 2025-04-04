import {useMutation} from "@tanstack/react-query";
import {useRef} from "react";
import ImportForm, {ImportFormValues} from "./ImportForm.tsx";

export default function ImportPage() {
    const fileRef = useRef<HTMLInputElement>(null);
    const accountRef = useRef<HTMLInputElement>(null);

    const mutation = useMutation({
        mutationFn: async (values: ImportFormValues) => {
            const data = new FormData();
            data.append("accountName", values.accountName);
            data.append("file", values.file!);

            await fetch('https://localhost:5001/import', {
                method: 'POST',
                body: data
            })
        }
    });

    return (
        <div className="p-2">
            <h1 className="text-3xl">Import Transactions</h1>
            <ImportForm onSubmit={mutation.mutateAsync}/>
        </div>
    );
}