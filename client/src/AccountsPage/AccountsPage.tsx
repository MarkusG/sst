import useGetAccounts from "./GetAccountsQuery.ts";
import LoadingIcon from "../LoadingIcon/LoadingIcon.tsx";
import AccountCard from "./AccountCard.tsx";
import NewAccountForm from "./NewAccountForm.tsx";

export default function AccountsPage() {
    const {data: accounts, isLoading, isError, error} = useGetAccounts();

    if (isLoading) {
        return (
            <div className="mt-16 w-min mx-auto">
                <LoadingIcon/>
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

    return (
        <div className="p-2">
            <h1 className="text-3xl">Accounts</h1>
            <div className="mb-2">
                <NewAccountForm/>
            </div>
            <div className="flex flex-col gap-2">
                {accounts.map(a => <AccountCard key={a.id} account={a}/>)}
            </div>
        </div>
    );
}