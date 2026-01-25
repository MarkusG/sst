import { CategoryTreeResponse } from "../../Contracts/Responses.ts";
import useAxios from "../../Hooks/Axios.ts";
import { useQuery, UseQueryOptions } from "@tanstack/react-query";

type Response = CategoryTreeResponse;

export default function useGetCategoryTree() {
  const axios = useAxios();

  const key = ["categories", "tree"];

  async function fn() {
    return await axios.get("/categories/tree").then((r) => r.data);
  }

  const options: Omit<UseQueryOptions<Response>, "queryKey" | "queryFn"> = {};

  return useQuery<Response>({
    queryKey: key,
    queryFn: fn,
    ...options,
  });
}
