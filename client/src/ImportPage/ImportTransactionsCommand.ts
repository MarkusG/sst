import useAxios, {SubmitFormOptions} from "../Hooks/Axios.ts";
import {useMutation} from "@tanstack/react-query";
import {useContext} from "react";
import {ImportContext} from "./ImportContext.tsx";

export interface ImportRequest {
    file: File,
    accountId: number
}

export default function useImportTransactions() {
    const axios = useAxios();
    const [context, _] = useContext(ImportContext);

    return useMutation({
        mutationFn: async () => await axios.post('/import', {file: context.file, accountId: context.accountId}, SubmitFormOptions)
    })
}