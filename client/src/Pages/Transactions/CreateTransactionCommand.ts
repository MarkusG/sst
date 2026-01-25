import useAxios from "../../Hooks/Axios.ts";
import { useMutation, useQueryClient } from "@tanstack/react-query";

export interface CreateTransactionCommand {
  timestamp: Date;
  amount: number;
  description: string;
  account: string;
  category?: string | null;
}

type Command = CreateTransactionCommand;

export default function useCreateTransaction() {
  const axios = useAxios();
  const queryClient = useQueryClient();

  async function fn(req: Command) {
    return await axios.post(`/transactions`, { ...req });
  }

  async function onSuccess() {
    await queryClient.invalidateQueries(["transactions"]);
  }

  return useMutation({
    mutationFn: fn,
    onSuccess: onSuccess,
  });
}
