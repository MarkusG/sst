import {ChangeEvent, useContext} from "react";
import {ImportContext} from "./ImportContext.tsx";

export default function Upload() {
    const [context, setContext] = useContext(ImportContext);

    if (context.files)
        return;

    function handleFileChange(e: ChangeEvent<HTMLInputElement>) {
        if (e.target.files && e.target.files.length > 0)
            setContext({...context, files: e.target.files});
    }

    return (
        <label htmlFor="upload" className="h-full flex flex-col items-center gap-2 text-gray-500 hover:text-black hover:cursor-pointer">
            <div className="w-full h-full flex flex-col justify-center items-center">
                <i className="fa fa-4x fa-upload mb-1"></i>
                <p className="text-2xl">Upload CSV</p>
            </div>
            <input id="upload" type="file" multiple className="hidden" onChange={handleFileChange}/>
        </label>
    );
}