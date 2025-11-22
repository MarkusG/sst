import useAxios from "../../Hooks/Axios.ts";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AccountFormValues } from "./AccountForm.tsx";

export default function useUpdateAccount(id: string) {
  const axios = useAxios();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (values: AccountFormValues) =>
      await axios.put(`/accounts/${id}`, values),
    onSuccess: async () => await queryClient.invalidateQueries(["accounts"]),
  });
}
