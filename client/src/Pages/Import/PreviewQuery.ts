import useAxios, { SubmitFormOptions } from "../../Hooks/Axios.ts";
import { useQuery, UseQueryOptions } from "@tanstack/react-query";
import { useContext } from "react";
import { ImportContext } from "./ImportContext.tsx";

export interface Preview {
  timestamp: Date;
  description: string;
  amount: number;
  skipped: boolean;
}

type Response = Preview[];

export default function usePreview() {
  const axios = useAxios();
  const [context, _] = useContext(ImportContext);

  const key = ["import", "preview", context.files];

  async function fn() {
    return await axios
      .post(
        "/import/preview",
        { files: context.files, accountId: context.accountId },
        SubmitFormOptions,
      )
      .then((r) => r.data);
  }

  const options: Omit<UseQueryOptions<Response>, "queryKey" | "queryFn"> = {
    enabled: !!context.files && !!context.accountId,
  };

  return useQuery<Response>({
    queryKey: key,
    queryFn: fn,
    ...options,
  });
}
