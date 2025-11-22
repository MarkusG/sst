import { SortOptions } from "../SortableTable/SortableTable";
import { QueryFunctionContext, useQuery } from "@tanstack/react-query";
import LoadingIcon from "../LoadingIcon/LoadingIcon";
import { QueryParameters } from "../QueryParameters";
import { ChangeEvent, useState } from "react";
import { TransactionsResponse } from "../Contracts/Responses";
import QueryControls from "./QueryControls/QueryControls";
import TransactionForm from "./TransactionForm";
import TransactionsTable from "./TransactionsTable.tsx";
import TransactionTableContextProvider from "./TransactionTableContext.tsx";

async function query({
  queryKey,
}: QueryFunctionContext): Promise<TransactionsResponse> {
  const params = queryKey[1] as QueryParameters;
  return await fetch(
    `https://localhost:5001/transactions${params.toString()}`,
  ).then((res) => res.json());
}

export default function TransactionsPage() {
  const [params, setParams] = useState(
    new QueryParameters({
      pageSize: 100,
      sortField: "timestamp",
      sortDirection: "up",
    }),
  );
  const [categorizingTransactionIdx, setCategorizingTransactionIdx] = useState<
    number | null
  >(null);

  const { data, isLoading, isError, error, isFetching } =
    useQuery<TransactionsResponse>({
      queryKey: ["transactions", params],
      keepPreviousData: true,
      queryFn: query,
    });

  if (isLoading) {
    return (
      <div className="mt-16 w-min mx-auto">
        <LoadingIcon />
      </div>
    );
  }

  if (isError) {
    return (
      <div className="p-2">
        <p className="text-xl">{error?.toString()}</p>
      </div>
    );
  }

  function previousPage() {
    if (params.page !== 1) {
      setParams(new QueryParameters({ ...params, page: params.page! - 1 }));
    }
  }

  function nextPage() {
    if (params.page !== data!.totalPages) {
      setParams(new QueryParameters({ ...params, page: params.page! + 1 }));
    }
  }

  function seekPage(e: ChangeEvent<HTMLInputElement>) {
    var page = Number(e.target.value);
    if (page > 0 && page <= data!.totalPages) {
      setParams(new QueryParameters({ ...params, page }));
    }
  }

  function sortUpdated({ field, direction }: SortOptions) {
    setParams(
      new QueryParameters({
        ...params,
        sortField: field,
        sortDirection: direction,
      }),
    );
  }

  async function paramsUpdated(params: QueryParameters) {
    setParams(params);
  }

  return (
    <div className="flex flex-col h-screen relative">
      {isFetching && (
        <>
          <div className="absolute top-0 left-0 w-full h-full bg-gray-100 opacity-50"></div>
          <div className="absolute top-0 left-0 w-full h-full flex items-center justify-center">
            <LoadingIcon />
          </div>
        </>
      )}
      <div className="px-2 pt-2">
        <h1 className="text-3xl">Transactions</h1>
      </div>
      <QueryControls params={params} onParamsUpdated={paramsUpdated} />
      <div className="px-2">
        <h2 className="text-xl">Add Transaction</h2>
        <TransactionForm />
      </div>
      <div className="overflow-auto">
        <TransactionTableContextProvider>
          <TransactionsTable
            options={{
              field: params.sortField,
              direction: params.sortDirection,
            }}
            onSortUpdated={sortUpdated}
            transactions={data.transactions}
          />
        </TransactionTableContextProvider>
      </div>
      <div className="flex justify-around items-baseline p-4 mt-auto">
        <button
          className="bg-white hover:bg-gray-50 disabled:bg-gray-200 disabled:text-gray-400 transition duration-300 p-2 rounded shadow"
          onClick={previousPage}
          disabled={data?.page === 1}
        >
          <i className="fa-solid fa-chevron-left"></i>
        </button>
        <div>
          <input
            type="number"
            value={params.page}
            onChange={seekPage}
            className="w-12 text-center appearance-none border shadow rounded"
          />{" "}
          / {data?.totalPages}
        </div>
        <button
          className="bg-white hover:bg-gray-50 disabled:bg-gray-200 disabled:text-gray-400 transition duration-300 p-2 rounded shadow"
          onClick={nextPage}
          disabled={data?.page === data?.totalPages}
        >
          <i className="fa-solid fa-chevron-right"></i>
        </button>
      </div>
    </div>
  );
}
