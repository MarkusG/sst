import useAxios from "../../Hooks/Axios.ts";
import { useQuery } from "@tanstack/react-query";

export interface Account {
  id: string;
  name: string;
  transactionCount: number;
  isPlaid: boolean;
}

export default function useGetAccounts() {
  const axios = useAxios();

  return useQuery<Account[]>({
    queryKey: ["accounts"],
    queryFn: async () => await axios.get("/accounts").then((r) => r.data),
  });
}
