import useAxios from "../../Hooks/Axios.ts";
import { useQuery, UseQueryOptions } from "@tanstack/react-query";
import { CashFlowResponse } from "../../Contracts/Responses.ts";

type Response = CashFlowResponse;

export default function useGetCashFlow(year: number) {
  const axios = useAxios();

  const key = ["cashflow", year];

  async function fn() {
    return await axios
      .get("/cashflow", { params: { year } })
      .then((r) => r.data);
  }

  const options: Omit<UseQueryOptions<Response>, "queryKey" | "queryFn"> = {};

  return useQuery<Response>({
    queryKey: key,
    queryFn: fn,
    ...options,
  });
}
