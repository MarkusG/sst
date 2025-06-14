// @ts-ignore
import plaidLogo from "../assets/plaid.svg";
import useUpdateAccount from "./UpdateAccountCommand.ts";
import {useState} from "react";
import AccountForm, {AccountFormValues} from "./AccountForm.tsx";
import Deleter from "./Deleter.tsx";

export interface AccountCardProps {
    account: Account
}

export interface Account {
    id: string,
    name: string,
    transactionCount: number,
    isPlaid: boolean
}

export default function AccountCard({account}: AccountCardProps) {
    const [editing, setEditing] = useState(false);
    const {mutateAsync: update} = useUpdateAccount(account.id);

    async function submit(values: AccountFormValues) {
        await update(values);
        setEditing(false);
    }

    return (
        <div className="p-2 bg-white shadow rounded flex justify-between items-center">
            <div>
                {!editing && <div className="flex gap-2">
                    <h2 className="text-2xl">{account.name}</h2>
                    <button onClick={() => setEditing(true)} title="Rename">
                        <i className="fa fa-pencil"></i>
                    </button>
                </div>}
                {editing && <AccountForm autoFocus={true} editing={true} initialValues={account} onSubmit={submit}/>}
                <p className="mb-1">{account.transactionCount} transactions</p>
                {account.isPlaid && <img src={plaidLogo} alt="Plaid logo" className="h-4"/>}
            </div>
            <div className="flex gap-2 items-center">
                <Deleter accountId={account.id} accountName={account.name}/>
            </div>
        </div>
    );
}