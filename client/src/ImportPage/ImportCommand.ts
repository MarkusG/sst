import useAxios, {SubmitFormOptions} from "../Hooks/Axios.ts";
import {useMutation} from "@tanstack/react-query";
import {useContext} from "react";
import {ImportContext} from "./ImportContext.tsx";

export default function useImport() {
    const axios = useAxios();
    const [context, _] = useContext(ImportContext);

    return useMutation({
        mutationFn: async () => await axios.post('/import', {files: context.files, accountId: context.accountId}, SubmitFormOptions)
    })
}