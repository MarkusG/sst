import useAxios, {SubmitFormOptions} from "../Hooks/Axios.ts";
import {useQuery} from "@tanstack/react-query";
import {useContext} from "react";
import {ImportContext} from "./ImportContext.tsx";

export interface Response {
    timestamp: Date,
    description: string,
    amount: number,
    skipped: boolean
}

export default function usePreview() {
    const axios = useAxios();
    const [context, _] = useContext(ImportContext);

    return useQuery<Response[]>({
        queryKey: ['import', 'preview', context.files],
        queryFn: async () => await axios.post('/import/preview', {files: context.files, accountId: context.accountId}, SubmitFormOptions).then(r => r.data),
        enabled: !!context.files && !!context.accountId
    });
}