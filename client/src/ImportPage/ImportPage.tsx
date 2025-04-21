import {ImportContextProvider} from "./ImportContext.tsx";
import Upload from "./Upload.tsx";
import Preview from "./Preview.tsx";
import SelectAccount from "./SelectAccount.tsx";
import Done from "./Done.tsx";

export default function ImportPage() {
    return (
        <ImportContextProvider>
            <Upload/>
            <Preview/>
            <SelectAccount/>
            <Done/>
        </ImportContextProvider>
    )
}