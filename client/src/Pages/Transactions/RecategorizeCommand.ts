import { useMutation, useQueryClient } from "@tanstack/react-query";
import useAxios from "../../Hooks/Axios.ts";

export interface RecategorizeCommand {
  id: number;
  categoryName: string;
}

type Command = RecategorizeCommand;

export default function useRecategorize() {
  const axios = useAxios();
  const queryClient = useQueryClient();

  async function fn({ id, categoryName }: Command) {
    await axios.put(`/categorizations/${id}`, { category: categoryName });
  }

  async function onSuccess() {
    await queryClient.invalidateQueries(["transactions"]);
  }

  return useMutation({
    mutationFn: fn,
    onSuccess: onSuccess,
  });
}
