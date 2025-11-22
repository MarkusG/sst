import { useContext, useState } from "react";
import { DefaultImportContext, ImportContext } from "./ImportContext.tsx";
import useGetImportAccounts from "./GetImportAccountsQuery.ts";
import LoadingIcon from "../LoadingIcon/LoadingIcon.tsx";

export default function SelectAccount() {
  const [context, setContext] = useContext(ImportContext);
  const [id, setId] = useState<number | null>();

  const { data: accounts, isLoading, isError, error } = useGetImportAccounts();

  if (!context.files || context.accountId) return;

  if (isLoading) {
    return (
      <div className="mt-16 w-min mx-auto">
        <LoadingIcon />
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

  async function submit() {
    await setContext({ ...context, accountId: id! });
  }

  return (
    <div className="p-2 h-full grid grid-rows-[min-content,_1fr,_min-content] grid-cols-1">
      <h1 className="text-2xl mb-2">Select Account</h1>
      <div className="flex flex-col gap-2 overflow-auto">
        {accounts.map((a) => (
          <div
            key={a.id}
            className={`p-2 shadow rounded flex justify-between items-center cursor-pointer ${id === a.id ? "bg-primary-200" : "bg-white hover:bg-primary-100"}`}
            onClick={() => setId(a.id)}
          >
            <div>
              <h2 className="text-2xl">{a.name}</h2>
              <p className="mb-1">{a.transactionCount} transactions</p>
            </div>
          </div>
        ))}
      </div>
      <div className="flex justify-around p-4">
        <button
          className="bg-gray-100 text-gray-700 px-16 py-4 rounded"
          onClick={reset}
        >
          Cancel
        </button>
        <button
          disabled={!id}
          className="bg-green-100 text-green-700 disabled:text-green-300 px-16 py-4 rounded"
          onClick={submit}
        >
          Import
        </button>
      </div>
    </div>
  );
}
