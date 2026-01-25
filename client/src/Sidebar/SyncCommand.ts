import { useMutation, useQueryClient } from "@tanstack/react-query";
import useAxios from "../Hooks/Axios.ts";

export interface SyncCommand {
  itemId: number;
}

type Command = SyncCommand;

export default function useSync() {
  const axios = useAxios();
  const queryClient = useQueryClient();

  async function fn({ itemId }: Command) {
    await axios.post(`/items/${itemId}/sync`);
  }

  async function onSuccess() {
    await queryClient.invalidateQueries(["accounts"]);
    await queryClient.invalidateQueries(["transactions"]);
  }

  return useMutation({
    mutationFn: fn,
    onSuccess: onSuccess,
  });
}
