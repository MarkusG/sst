import useAxios from "../../Hooks/Axios.ts";
import { useQuery } from "@tanstack/react-query";

export interface Account {
  id: number;
  name: string;
  transactionCount: number;
}

export default function useGetImportAccounts() {
  const axios = useAxios();

  return useQuery<Account[]>({
    queryKey: ["import", "accounts"],
    queryFn: async () =>
      await axios.get("/import/accounts").then((r) => r.data),
  });
}
