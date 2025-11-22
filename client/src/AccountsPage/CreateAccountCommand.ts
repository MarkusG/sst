import useAxios from "../Hooks/Axios.ts";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { AccountFormValues } from "./AccountForm.tsx";

export default function useCreateAccount() {
  const axios = useAxios();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async (values: AccountFormValues) =>
      await axios.post("/accounts", values),
    onSuccess: async () => await queryClient.invalidateQueries(["accounts"]),
  });
}
