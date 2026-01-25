import useAxios from "../../Hooks/Axios.ts";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { CategoryTreeEntryResponse } from "../../Contracts/Responses.ts";

export default function useUpdateCategory() {
  const axios = useAxios();
  const queryClient = useQueryClient();

  async function fn(c: CategoryTreeEntryResponse) {
    await axios.put(`/categories/${c.id}`, { ...c });
  }

  async function onSuccess() {
    await queryClient.invalidateQueries(["categories", "tree"]);
  }

  return useMutation({
    mutationFn: fn,
    onSuccess: onSuccess,
  });
}
