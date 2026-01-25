import useAxios from "../../Hooks/Axios.ts";
import { useMutation, useQueryClient } from "@tanstack/react-query";

export interface CreateCategoryCommand {
  name: string;
}

type Command = CreateCategoryCommand;

export default function useCreateCategory() {
  const axios = useAxios();
  const queryClient = useQueryClient();

  async function fn(req: Command) {
    await axios.post(`/categories`, { name: req.name });
  }

  async function onSuccess() {
    await queryClient.invalidateQueries(["categories", "tree"]);
  }

  return useMutation({
    mutationFn: fn,
    onSuccess: onSuccess,
  });
}
