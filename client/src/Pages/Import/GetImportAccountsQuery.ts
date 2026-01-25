import useAxios from "../../Hooks/Axios.ts";
import { useQuery, UseQueryOptions } from "@tanstack/react-query";

export interface Account {
  id: number;
  name: string;
  transactionCount: number;
}

type Response = Account[];

export default function useGetImportAccounts() {
  const axios = useAxios();

  const key = ["import", "accounts"];

  async function fn() {
    return await axios.get("/import/accounts").then((r) => r.data);
  }

  const options: Omit<UseQueryOptions<Response>, "queryKey" | "queryFn"> = {};

  return useQuery<Response>({
    queryKey: key,
    queryFn: fn,
    ...options,
  });
}
