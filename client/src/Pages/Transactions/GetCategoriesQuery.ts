import { CategoriesResponse } from "../../Contracts/Responses.ts";
import useAxios from "../../Hooks/Axios.ts";
import { useQuery, UseQueryOptions } from "@tanstack/react-query";

type Response = CategoriesResponse;

export default function useGetCategories() {
  const axios = useAxios();

  const key = ["categories"];

  async function fn() {
    return await axios.get("/categories").then((r) => r.data);
  }

  const options: Omit<UseQueryOptions<Response>, "queryKey" | "queryFn"> = {};

  return useQuery<Response>({
    queryKey: key,
    queryFn: fn,
    ...options,
  });
}
