import useAxios from "../../Hooks/Axios.ts";
import { useQuery, UseQueryOptions } from "@tanstack/react-query";
import { TransactionsResponse } from "../../Contracts/Responses.ts";
import { QueryParameters } from "../../QueryParameters.ts";

type Response = TransactionsResponse;

export default function useGetTransactions(params: QueryParameters) {
  const axios = useAxios();

  const key = ["transactions", params];

  async function fn() {
    return await axios
      .get("/transactions", { params: params })
      .then((r) => r.data);
  }

  const options: Omit<UseQueryOptions<Response>, "queryKey" | "queryFn"> = {
    keepPreviousData: true,
  };

  return useQuery<Response>({
    queryKey: key,
    queryFn: fn,
    ...options,
  });
}
