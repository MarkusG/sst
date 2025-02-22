import SortableTable, { SortOptions } from "../SortableTable/SortableTable";
import SortableHeaderCell from "../SortableTable/SortableHeaderCell";
import TransactionRow from "./TransactionRow";
import { QueryFunctionContext, useQuery } from "@tanstack/react-query";
import LoadingIcon from "../LoadingIcon/LoadingIcon";
import { QueryParameters } from "../QueryParameters";
import { ChangeEvent, useState } from "react";
import { TransactionsResponse } from "../Contracts/Responses";
import QueryControls from "./QueryControls/QueryControls";
import TransactionForm from "./TransactionForm";

async function query({ queryKey }: QueryFunctionContext) : Promise<TransactionsResponse> {
    const params = queryKey[1] as QueryParameters;
    return await fetch(`https://localhost:5001/transactions${params.toString()}`)
        .then((res) => res.json())
}

export default function TransactionsPage() {
    const [params, setParams] = useState(new QueryParameters({ pageSize: 100, sortField: "timestamp", sortDirection: "up" }));
    const [categorizingTransactionId, setCategorizingTransactionId] = useState<number | null>(null);

    const { data, error, isLoading, isFetching } = useQuery<TransactionsResponse>({
        queryKey: ['transactions', params],
        keepPreviousData: true,
        queryFn: query
    });

    if (isLoading) {
        return (
            <div className="mt-16 w-min mx-auto">
                <LoadingIcon/>
            </div>
        );
    }

    if (!!error) {
        return (
            <div className="p-2">
                <p className="text-xl">{error.toString()}</p>
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
        setParams(new QueryParameters({ ...params, sortField: field, sortDirection: direction }));
    }

    async function paramsUpdated(params: QueryParameters) {
        setParams(params);
    }

    function categorized(id: number, moveNext: boolean) {
        if (!moveNext) {
            setCategorizingTransactionId(null);
            return;
        }

        const idx = data?.transactions.findIndex((t) => t.id === id) ?? -1;
        if (idx === -1)
            return;
        const next = data?.transactions[idx + 1];
        setCategorizingTransactionId(next?.id ?? null);
    }

    return (
        <div className="flex flex-col h-screen">
            <div className="px-2 pt-2">
                <h1 className="text-3xl">Transactions</h1>
            </div>
            <QueryControls params={params} onParamsUpdated={paramsUpdated}/>
            <div className="px-2">
                <h2 className="text-xl">Add Transaction</h2>
                <TransactionForm/>
            </div>
            <div className="overflow-auto relative">
                <SortableTable className="w-full table-fixed border-separate border-spacing-0 whitespace-nowrap"
                    options={{ field: params.sortField, direction: params.sortDirection }}
                    onSortUpdated={sortUpdated}>
                    <thead className="sticky top-0 bg-gray-50 border-b">
                        <tr>
                            <SortableHeaderCell field="timestamp" className="w-[160px] px-1 pl-2 border-gray-300 border-r border-b">Timestamp</SortableHeaderCell>
                            <SortableHeaderCell field="account" className="w-[200px] px-1 border-gray-300 border-r border-b">Account</SortableHeaderCell>
                            <SortableHeaderCell field="description" className="px-1 border-gray-300 border-r border-b">Description</SortableHeaderCell>
                            <SortableHeaderCell field="amount" className="w-[100px] px-1 border-gray-300 border-r border-b">Amount</SortableHeaderCell>
                            <SortableHeaderCell field="category" className="w-[200px] px-1 border-gray-300 border-r border-b">Category</SortableHeaderCell>
                        </tr>
                    </thead>
                    <tbody>
                        {data?.transactions.map(t =>
                            <TransactionRow
                                transaction={t}
                                isCategorizing={t.id === categorizingTransactionId}
                                onCategorized={categorized}
                                key={t.id}/>
                        )}
                    </tbody>
                </SortableTable>
                {isFetching &&
                    <>
                        <div className="absolute top-0 left-0 w-full h-full bg-gray-100 opacity-50"></div>
                        <div className="absolute top-0 left-0 w-full h-full flex items-center justify-center">
                            <LoadingIcon/>
                        </div>
                    </>
                }
            </div>
            <div className="flex justify-around items-baseline p-4 mt-auto">
                <button className="bg-white hover:bg-gray-50 disabled:bg-gray-200 disabled:text-gray-400 transition duration-300 p-2 rounded shadow"
                    onClick={previousPage}
                    disabled={data?.page === 1}>
                    <i className="fa-solid fa-chevron-left"></i>
                </button>
                <div>
                    <input type="number" value={params.page} onChange={seekPage} className="w-12 text-center appearance-none border shadow rounded"/> / {data?.totalPages}
                </div>
                <button className="bg-white hover:bg-gray-50 disabled:bg-gray-200 disabled:text-gray-400 transition duration-300 p-2 rounded shadow"
                    onClick={nextPage}
                    disabled={data?.page === data?.totalPages}>
                    <i className="fa-solid fa-chevron-right"></i>
                </button>
            </div>
        </div>
    );
}
