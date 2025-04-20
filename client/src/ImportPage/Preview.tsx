import {useContext} from "react";
import {DefaultImportContext, ImportContext} from "./ImportContext.tsx";
import usePreview from "./PreviewQuery.ts";
import LoadingIcon from "../LoadingIcon/LoadingIcon.tsx";
import Amount from "../Amount.tsx";
import Timestamp from "../Timestamp.tsx";
import useImportTransactions from "./ImportTransactionsCommand.ts";

export default function Preview() {
    const [context, setContext] = useContext(ImportContext);

    const {data, isLoading, isError, error} = usePreview();
    const {mutateAsync} = useImportTransactions();

    if (!context.accountId || context.done)
        return;

    if (isLoading) {
        return (
            <div className="mt-16 w-min mx-auto">
                <LoadingIcon/>
            </div>
        );
    }

    if (isError) {
        if (isError) {
            return (
                <div className="p-2">
                    <p className="text-xl">{error?.toString()}</p>
                </div>
            );
        }
    }

    function reset() {
        setContext(DefaultImportContext);
    }

    async function approve() {
        await mutateAsync();
        setContext({...context, done: true});
    }

    return (
        <div className="m-2 bg-white shadow rounded h-[calc(100vh_-_1rem)] grid grid-rows-[min-content_1fr_min-content] grid-cols-1">
            <div className="p-2">
                <h1 className="text-3xl">Transactions to Import</h1>
                <p>Previously imported transactions have been marked with a strike and will not be imported.</p>
            </div>
            <div className="overflow-auto overflow-x-hidden">
                <table className="w-full table-fixed border-separate border-spacing-0 whitespace-nowrap">
                    <thead>
                    <tr>
                        <td className="w-[160px] px-1 pl-2 border-gray-300 border-r border-b">Timestamp</td>
                        <td className="px-1 border-gray-300 border-r border-b">Description</td>
                        <td className="w-[140px] px-1 pr-2 border-gray-300 border-b">Amount</td>
                    </tr>
                    </thead>
                    <tbody>
                    {data && data.map(t =>
                        <tr key={`${t.timestamp} ${t.description} ${t.amount}`} className={`even:bg-gray-100 ${t.skipped ? 'line-through' : ''}`}>
                            <td className="px-1 pl-2"><Timestamp ts={t.timestamp}/></td>
                            <td className="overflow-auto text-ellipsis">{t.description}</td>
                            <td className="px-1 pr-2 text-right"><Amount amount={t.amount}/></td>
                        </tr>
                    )}
                    </tbody>
                </table>
            </div>
            <div className="flex justify-around p-4">
                <button className="bg-gray-100 text-gray-700 px-16 py-4 rounded" onClick={reset}>Cancel</button>
                <button className="bg-green-100 text-green-700 px-16 py-4 rounded" onClick={approve}>Approve</button>
            </div>
        </div>
    );
}