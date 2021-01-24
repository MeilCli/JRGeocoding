# Japanese Reverse Geocoding
日本限定の逆ジオコーディングツールです。緯度経度から住所がわかります

## 使用してるデータについて
[政府統計の総合窓口(e-Stat)](https://www.e-stat.go.jp/)の国勢調査2015年境界データ小地域(町丁・字等別)を使用しています

境界データの品質については[このページ](https://www.e-stat.go.jp/help/data-definition-information/download)に記載されてます。要約すると国勢調査の調査のために設定された調査区の境界をもとに作成されてるので実際の住所や境界などとは多少の差があるということです

たとえば京都市などでは実際の住所と差異がある箇所が見られると思います

## 試しに使ってみる
試しに使ってみる用にNetlifyで確認用データを公開しています。試用のために使ってください(使いすぎると通信量で制限されます)

### ブラウザから
[https://jrgeocoding.meilcli.net/](https://jrgeocoding.meilcli.net/)

### C#の場合
#### コマンドライン
```
$ dotnet run --project ./csharp/JRGeocoding.Console/JRGeocoding.Console.csproj -- 35.681615 139.764956
東京都千代田区丸の内１丁目
```

### TypeScriptの場合
#### コマンドライン
```
$ npm run main -- 35.681615 139.764956
東京都千代田区丸の内１丁目
```

## 使用準備
境界データをそのまま処理するにはデータが多すぎるため中間ファイルを生成するようにしています。また、中間ファイルを公開するにはデータ量が多すぎるため、利用者が各自で中間ファイルを生成する形式を取っています

### 推奨環境
中間ファイルの生成にはそこそこのPCスペックが求められます

- 高いシングルコア性能のあるCPU(たとえばCore i7やCore i9)
- 高いランダムライト性能のあるストレージ(たとえばSSD)
- 容量の多いRAM(たとえば32GB)

MeilCliの環境ではCore i9 10900kで中間ファイル生成に約80秒かかりました(**約3GBのフォルダーが生成されるので注意**)

### 中間ファイル生成
1. [.NET 5.0](https://dotnet.microsoft.com/download)をインストール
1. このリポジトリーをクローン
1. ターミナルでクローンしたパスを開く
1. `dotnet run --project ./generator/generator.csproj`を実行
1. `public/data`フォルダーが生成されていたら成功

## 使用方法
### C#の場合
#### コマンドライン
```
$ dotnet run --project ./csharp/JRGeocoding.Console/JRGeocoding.Console.csproj -- 35.681615 139.764956 --local
東京都千代田区丸の内１丁目
```

#### ライブラリー
ToDo: Write

### TypeScriptの場合
#### コマンドライン
```
$ npm install
$ npm run main -- 35.681615 139.764956 --local
東京都千代田区丸の内１丁目
```

#### ライブラリー
ToDo: Write

## Publish for MeilCli
1. `npm install -g netlify-cli`
1. `dotnet run --project ./generator/generator.csproj`
1. `npm run build`
1. `npm run pack`
1. `netlify deploy --dir=public`
1. `netlify deploy --dir=public --prod`

## ライセンス
MIT Licenseで公開しています

### 使用しているOSS
- generator:
  - [SpanJson](https://github.com/Tornhoof/SpanJson), MIT License
- csharp:
  - [Utf8Json](https://github.com/neuecc/Utf8Json), MIT License
- typescript:
  - [node-fetch](https://github.com/node-fetch/node-fetch), MIT License