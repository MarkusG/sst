import useAxios from "../Hooks/Axios.ts";
import { useMutation, useQueryClient } from "@tanstack/react-query";

export default function useDeleteAccount(id: string) {
  const axios = useAxios();
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: async () => await axios.delete(`/accounts/${id}`),
    onSuccess: async () => await queryClient.invalidateQueries(["accounts"]),
  });
}
