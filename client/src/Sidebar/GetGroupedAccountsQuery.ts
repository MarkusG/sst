import { useQuery, UseQueryOptions } from "@tanstack/react-query";
import { AccountsResponse } from "../Contracts/Responses.ts";
import useAxios from "../Hooks/Axios.ts";

type Response = AccountsResponse;

export default function useGetGroupedAccounts() {
  const axios = useAxios();

  const key = ["accounts", "grouped"];

  async function fn() {
    return await axios.get("/accounts/grouped").then((r) => r.data);
  }

  const options: Omit<UseQueryOptions<Response>, "queryKey" | "queryFn"> = {};

  return useQuery<Response>({
    queryKey: key,
    queryFn: fn,
    ...options,
  });
}
