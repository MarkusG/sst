function TransactionRow() {
    return (
        <tr className="border-b border-gray-300">
            <td className="px-1 pl-4">{`${(new Date()).toLocaleDateString()} ${(new Date()).toLocaleTimeString()}`}</td>
            {Math.random() < 0.5 ? (<td className="px-1 text-right text-red-500">-$1,000.00</td>) : (<td className="text-right px-1">$2,000,000.00</td>)}
            <td className="px-1">Wikimedia San Francisco CA Wikimedia San Francisco CA</td>
            <td className="px-1">Chase Checking</td>
            <td className="px-1">Donations</td>
            <td className="px-1">Wikipedia Donation</td>
            <td className="px-1">foo, bar, baz, foo, bar, baz</td>
        </tr>
    );
}

export default TransactionRow;
