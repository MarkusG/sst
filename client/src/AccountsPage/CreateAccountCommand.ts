import useAxios from "../Hooks/Axios.ts";
import {useMutation, useQueryClient} from "@tanstack/react-query";
import {NewAccountFormValues} from "./NewAccountForm.tsx";

export default function useCreateAccount() {
    const axios = useAxios();
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (values: NewAccountFormValues) => await axios.post('/accounts', values),
        onSuccess: async () => await queryClient.invalidateQueries(['accounts'])
    });
}