import { useMutation, useQueryClient } from "@tanstack/react-query";
import useAxios from "../../Hooks/Axios.ts";

export interface UncategorizeCommand {
  id: number;
}

type Command = UncategorizeCommand;

export default function useUncategorize() {
  const axios = useAxios();
  const queryClient = useQueryClient();

  async function fn({ id }: Command) {
    await axios.delete(`/categorizations/${id}`);
  }

  async function onSuccess() {
    await queryClient.invalidateQueries(["transactions"]);
  }

  return useMutation({
    mutationFn: fn,
    onSuccess: onSuccess,
  });
}
