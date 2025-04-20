// @ts-ignore
import plaidLogo from "../../public/plaid.svg";

export interface AccountCardProps {
    account: Account
}

export interface Account {
    name: string,
    transactionCount: number,
    isPlaid: boolean
}

export default function AccountCard({account}: AccountCardProps) {
    return (
        <div className="p-2 bg-white shadow rounded flex justify-between items-center">
            <div>
                <h2 className="text-2xl">{account.name}</h2>
                <p>{account.transactionCount} transactions</p>
            </div>
            {account.isPlaid && <img src={plaidLogo} alt="Plaid logo" className="h-8"/>}
        </div>
    );
}