import { useMutation, useQueryClient } from "@tanstack/react-query";
import useAxios from "../../Hooks/Axios.ts";

export interface CategorizeCommand {
  transactionId: number;
  categoryName: string;
  amount: number;
}

type Command = CategorizeCommand;

export default function useCategorize() {
  const axios = useAxios();
  const queryClient = useQueryClient();

  async function fn({ transactionId, categoryName, amount }: Command) {
    await axios.put(
      `/transactions/${transactionId}/categorizations/${categoryName}`,
      { category: categoryName, amount, position: 0 },
    );
  }

  async function onSuccess() {
    await queryClient.invalidateQueries(["transactions"]);
  }

  return useMutation({
    mutationFn: fn,
    onSuccess: onSuccess,
  });
}
