import useAxios from "../../Hooks/Axios.ts";
import { useQuery, UseQueryOptions } from "@tanstack/react-query";

export interface Account {
  id: string;
  name: string;
  transactionCount: number;
  isPlaid: boolean;
}

type Response = Account[];

export default function useGetAccounts() {
  const axios = useAxios();

  const key = ["accounts"];

  async function fn() {
    return await axios.get("/accounts").then((r) => r.data);
  }

  const options: Omit<UseQueryOptions<Response>, "queryKey" | "queryFn"> = {};

  return useQuery<Response>({
    queryKey: key,
    queryFn: fn,
    ...options,
  });
}
